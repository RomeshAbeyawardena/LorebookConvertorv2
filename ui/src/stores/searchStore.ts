import { defineStore } from "pinia";
import { useEntryStore } from "./entryStore";
import { IMappedIndex } from "../models/mapped-index";
import { computed, ComputedRef, ref, Ref } from "vue";
import { ILorebookGroup } from "../models/groups";
import { ISearchIndex } from "../models/searchIndex";
import { useEntryGroupingStore } from "./EntryGroupingStore";
import { useStoryStore } from "./storyStore";
import { IEntry } from "../models/entry";
import { orderBy } from "lodash";
export interface ISearchStore {
    filteredCategories:ComputedRef<Array<ILorebookGroup>>;
    getOrAddSearchIndex:ComputedRef<Array<ISearchIndex>>;
    getOrMapIndexes:ComputedRef<Array<IMappedIndex>>;
    generateSearchIndex:() => void;
    isMapped:Ref<boolean>;
    isSearchIndexLoaded:Ref<boolean>;
    mappedIndexes:Ref<Array<IMappedIndex>>;
    mapIndexes():Array<IMappedIndex>;
    searchIndex:Ref<Array<ISearchIndex>>;
    searchText:Ref<string>;
    selectedSearchItem:Ref<string>;
}


export const useSearchStore = defineStore("search-store", (): ISearchStore => {
    const entryStore = useEntryStore();
    const isMapped = ref(false);
    const mappedIndexes = ref<Array<IMappedIndex>>([]);
    const isSearchIndexLoaded = ref(false);
    const searchIndex = ref<Array<ISearchIndex>>([]);

    function generateSearchIndex() {
        const lorebook = entryStore.lorebook;
        const isLorebookLoaded = entryStore.isLorebookLoaded;

        if(isLorebookLoaded && !isSearchIndexLoaded.value) {
            searchIndex.value =[];
            searchIndex.value.push(... lorebook.Categories.map((c) => {
                return {
                    id: c.Id,
                    keys: [c.Name],
                    title: c.Name,
                    summary: c.Name,
                    category: c
                } as ISearchIndex
            }));

            searchIndex.value.push(... lorebook.Entries.map(e => {
                return {
                    id: e.Id.concat(",",e.CategoryId),
                    keys: [e.DisplayName,... e.Keys],
                    title: e.DisplayName,
                    summary: e.Text.substring(0, 50),
                    entry: e
                } as ISearchIndex
            }));
            
            isSearchIndexLoaded.value = true;
        }
        return searchIndex.value;
    }

    const getOrAddSearchIndex = computed(() => {
        return generateSearchIndex();
    })
    const storyStore = useStoryStore();
    const entryGroupingStore = useEntryGroupingStore();
    const searchText = ref("");
    const filteredCategories = computed(() => {
        if(!entryStore.isLorebookLoaded)
        {
            return [];
        }

        if(searchText.value.length > 3) {
            return entryStore.lorebook.Groupings.filter(f => f.Category.Name
                .toLocaleLowerCase().includes(
                    searchText.value.toLocaleLowerCase()));
        }
        else {
            const allGroups:Array<ILorebookGroup> =[]
            if(storyStore.selectedStory)
            {
                const groups = entryGroupingStore.groups
                
                if(groups.length > 0) {
                    allGroups.push(... entryStore.lorebook.Groupings)

                    for(let group of groups) {
                        allGroups.push({
                            Category: {
                                Id: "",
                                Name:group.name,
                            },
                            groupId: group.groupId,
                            CategoryId: "",
                            Entries: group.entryIds.map(e => entryStore.lorebook.Entries.find(le => le.Id == e) as IEntry)
                        });
                    }
                    
                    return orderBy(allGroups, [a => a.Category.Name], "asc");
                }
            }
            
            return entryStore.lorebook.Groupings;
        }
    })

    const selectedSearchItem = ref("");

    function mapIndexes()
    {
        if(isMapped.value) {
            return mappedIndexes.value;
        }
        
        if (!entryStore.isLorebookLoaded)
        {
            return [];
        }

        const mapped = entryStore.lorebook.Groupings
            .flatMap((g, categoryIndex) => {
                return g.Entries.map((e, entryIndex)=> {
                    return {
                        categoryIndex: categoryIndex,
                        entryIndex:entryIndex,
                        id: e.Id
                    }
                });
        });

        mappedIndexes.value.push(... mapped);
        isMapped.value = true;
        return mappedIndexes.value;
    }

    const getOrMapIndexes = computed(() => mapIndexes()); 

    return {
        isMapped,
        filteredCategories,
        getOrAddSearchIndex,
        generateSearchIndex,
        getOrMapIndexes,
        searchIndex,
        searchText,
        selectedSearchItem,
        isSearchIndexLoaded,
        mapIndexes,
        mappedIndexes
    };
});